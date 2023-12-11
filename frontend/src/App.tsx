import "./styles/css/index.css"
import { Header } from "./modules/Header"
import { Route, Routes } from "react-router-dom";
import { PublicPage } from "./pages/PublicPage";
import { LoginPage } from "./pages/LoginPage";
import { InvalidPage } from "./pages/InvalidPage";
import { RegisterPage } from "./pages/RegisterPage";
import { EditActions, Paths } from "./helpers/enums";
import ProfilePage from "./pages/ProfilePage";
import EditUserPage from "./pages/EditUserPage";
import { useEffect, useState } from "react";
import { fetchUser, resetUserState } from "./store/actions/userActionCreators";
import { useAppDispatch, useAppSelector } from "./hooks/redux";
import { ErrorModal } from "./components/ErrorModal";
import { validate } from "./helpers/validation";
import Footer from "./modules/Footer";


function App() {
    const dispatch = useAppDispatch();
    const { user } = useAppSelector(state => state.userReducer);
    const { isAuthenticated } = useAppSelector(state => state.authReducer);
    const [isErrorModalActive, setErrorModalActive] = useState(false);

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
                <Route path={Paths.editUserPath} element={<EditUserPage action={EditActions.editUser} />}></Route>
                <Route path={Paths.editPasswordPath} element={<EditUserPage action={EditActions.editPassword} />}></Route>
                <Route path="*" element={<InvalidPage />}></Route>
            </Routes>

            <Footer />

            {/* <ErrorModal active={isErrorModalActive} error={error} onClose={closeModal} /> */}
        </>
    );
}



export default App
