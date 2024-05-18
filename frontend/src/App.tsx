import "./styles/css/index.css"
import { Header } from "./modules/Header"
import { Route, Routes } from "react-router-dom";
import { PublicPage } from "./pages/PublicPage";
import { LoginPage } from "./pages/LoginPage";
import { InvalidPage } from "./pages/InvalidPage";
import { RegisterPage } from "./pages/RegisterPage";
import ProfilePage from "./pages/ProfilePage";
import Footer from "./components/Footer";
import CoursePage from "./pages/CoursePage";
import UserCoursesPage from "./pages/UserCoursesPage";
import { paths } from "./models/paths";
import { useAppDispatch, useAppSelector } from "./hooks/redux";
import { useEffect } from "react";
import { fetchUser } from "./store/actions/userActionCreators";
import CourseParticipantsPage from "./pages/CourseParticipantsPage";
import CourseRolesPage from "./pages/CourseRolesPage.tsx";
import Layout from "./pages/Layout.tsx";
import { CourseContext, CourseState } from "./contexts/CourseContext.tsx";


function App() {
    const dispatch = useAppDispatch();
    const { user } = useAppSelector(state => state.userReducer);
    const { isAuthenticated } = useAppSelector(state => state.authReducer);

    useEffect(() => {
        if (isAuthenticated && !user) dispatch(fetchUser());

    }, [dispatch, isAuthenticated, user]);

    return (
        <>
            <Header />

            <Routes>
                <Route path={'/'} element={<Layout />}></Route>
                <Route path={paths.public(':pageNumber')} element={<PublicPage />}></Route>
                <Route path={paths.login} element={<LoginPage />}></Route>
                <Route path={paths.register} element={<RegisterPage />}></Route>
                <Route path={paths.profile.edit(':action')} element={<ProfilePage />}></Route>
                <Route path={paths.profile.courses(':type', ':pageNumber')} element={<UserCoursesPage />} />
                <Route path={paths.course.view(':courseId')} element={<CoursePage />}></Route>
                <Route path={paths.course.participants(':courseId', ':pageNumber')} element={<CourseParticipantsPage />} />
                <Route path={paths.course.roles(':courseId')} element={<CourseRolesPage />}></Route>
                <Route path="*" element={<InvalidPage />}></Route>
            </Routes>

            <Footer />
        </>

    );
}



export default App
