import "./styles/css/index.css"
import { Header } from "./modules/Header"
import { Route, Routes } from "react-router-dom";
import { PublicPage } from "./pages/PublicPage";
import { LoginPage } from "./pages/LoginPage";
import { InvalidPage } from "./pages/InvalidPage";
import { RegisterPage } from "./pages/RegisterPage";
import ProfilePage from "./pages/ProfilePage";
import Footer from "./modules/Footer.tsx";
import CoursePage from "./pages/CoursePage";
import UserCoursesPage from "./pages/UserCoursesPage";
import { paths } from "./models/paths";
import { useAppDispatch, useAppSelector } from "./hooks/redux";
import { useEffect } from "react";
import { fetchUser, resetUserState } from "./store/actions/userActionCreators";
import CourseParticipantsPage from "./pages/CourseParticipantsPage";
import CourseRolesPage from "./pages/CourseRolesPage.tsx";
import CourseNestedLayout from "./pages/CourseNestedLayout.tsx";
import ModuleNestedLayout from "./pages/ModuleNestedLayout.tsx";
import ModulePage from "./pages/ModulePage.tsx";
import LessonNestedLayout from "./pages/LessonNestedLayout.tsx";
import LessonPage from "./pages/LessonPage.tsx";
import LessonEditPage from "./pages/LessonEditPage.tsx";
import HomePage from "./pages/HomePage.tsx";
import PrivacyPolicy from "./pages/PrivacyPolicy.tsx";
import { ErrorModal } from "./components/Modal/ErrorModal.tsx";
import PageLoader from "./components/Loader/PageLoader.tsx";


function App() {
    const dispatch = useAppDispatch();
    const { user, error, loading } = useAppSelector(state => state.userReducer);
    const { isAuthenticated } = useAppSelector(state => state.authReducer);

    useEffect(() => {
        if (!user && isAuthenticated) dispatch(fetchUser());
    }, [user]);

    return (
        <>
            <Header />

            <Routes>
                <Route path={'/'} element={<HomePage />}></Route>
                <Route path={paths.public(':pageNumber')} element={<PublicPage />}></Route>
                <Route path={paths.login} element={<LoginPage />}></Route>
                <Route path={paths.register} element={<RegisterPage />}></Route>
                <Route path={paths.profile.edit(':action')} element={<ProfilePage />}></Route>
                <Route path={paths.profile.courses(':type', ':pageNumber')} element={<UserCoursesPage />} />
                <Route path={paths.course.base(':courseId')} element={<CourseNestedLayout />}>
                    <Route path={paths.course.view.base} element={<CoursePage />}></Route>
                    <Route path={paths.course.participants.base(':pageNumber')} element={<CourseParticipantsPage />} />
                    <Route path={paths.course.roles.base} element={<CourseRolesPage />}></Route>
                    <Route path={paths.module.base(':moduleId')} element={<ModuleNestedLayout />}>
                        <Route path={paths.module.view.base} element={<ModulePage />}></Route>
                        <Route path={paths.lesson.base(':lessonId')} element={<LessonNestedLayout />}>
                            <Route path={paths.lesson.view.base} element={<LessonPage />}></Route>
                            <Route path={paths.lesson.edit.base} element={<LessonEditPage />}></Route>
                        </Route>
                    </Route>
                </Route>
                <Route path={paths.privacy.base} element={<PrivacyPolicy />}></Route>
                <Route path="*" element={<InvalidPage />}></Route>
            </Routes >

            {error && isAuthenticated &&
                <ErrorModal active={Boolean(error)} error={error} onClose={() => dispatch(resetUserState())} />
            }

            {loading && !error &&
                <PageLoader />
            }

            <Footer />
        </>

    );
}



export default App
