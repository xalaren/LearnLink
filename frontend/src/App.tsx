import "./styles/css/index.css"
import { Header } from "./modules/Header"
import { Route, Routes } from "react-router-dom";
import { PublicPage } from "./pages/PublicPage";
import { LoginPage } from "./pages/LoginPage";
import { InvalidPage } from "./pages/InvalidPage";
import { RegisterPage } from "./pages/RegisterPage";
import { EditActions } from "./models/enums";
import ProfilePage from "./pages/ProfilePage";
import EditUserPage from "./pages/EditUserPage";
import { useEffect } from "react";
import { fetchUser } from "./store/actions/userActionCreators";
import { useAppDispatch, useAppSelector } from "./hooks/redux";
import Footer from "./modules/Footer";
import CoursePage from "./pages/CoursePage";
import UserCoursesPage from "./pages/UserCoursesPage";
import { Paths } from "./models/paths";
import ModulePage from "./pages/ModulePage";


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
                <Route path={Paths.homePath} element={<PublicPage />}></Route>
                <Route path={Paths.loginPath} element={<LoginPage />}></Route>
                <Route path={Paths.registerPath} element={<RegisterPage />}></Route>
                <Route path={Paths.profilePath} element={<ProfilePage />}></Route>
                <Route path={Paths.userCoursesPath + '/:type'} element={<UserCoursesPage />}></Route>
                <Route path={Paths.editUserPath} element={<EditUserPage action={EditActions.editUser} />}></Route>
                <Route path={Paths.editPasswordPath} element={<EditUserPage action={EditActions.editPassword} />}></Route>
                <Route path={Paths.courseViewFullPath} element={<CoursePage />}></Route>
                <Route path={Paths.moduleViewFullPath} element={<ModulePage />}></Route>
                <Route path="*" element={<InvalidPage />}></Route>
            </Routes >

            <Footer />

            {/* <ErrorModal active={isErrorModalActive} error={error} onClose={closeModal} /> */}
        </>
    );
}



export default App
