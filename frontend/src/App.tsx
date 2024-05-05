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
import { Paths } from "./models/paths";
import { useAppDispatch, useAppSelector } from "./hooks/redux";
import { useEffect } from "react";
import { fetchUser } from "./store/actions/userActionCreators";
import HomePage from "./pages/HomePage";
import CourseParticipantsPage from "./pages/CourseParticipantsPage";


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
                <Route path={'/'} element={<HomePage />}></Route>
                <Route path={Paths.publicPath + '/:pageNumber'} element={<PublicPage />}></Route>
                <Route path={Paths.loginPath} element={<LoginPage />}></Route>
                <Route path={Paths.registerPath} element={<RegisterPage />}></Route>
                <Route path={Paths.profilePath + '/edit/:action'} element={<ProfilePage />}></Route>
                <Route path={Paths.userCoursesFullPath + '/:pageNumber'} element={<UserCoursesPage />} />
                <Route path={Paths.courseViewFullPath} element={<CoursePage />}></Route>
                <Route path={Paths.courseParticipantsPath + '/:pageNumber'} element={<CourseParticipantsPage />}></Route>
                <Route path="*" element={<InvalidPage />}></Route>
            </Routes >

            <Footer />
        </>
    );
}



export default App
