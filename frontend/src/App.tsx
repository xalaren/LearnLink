import "./styles/css/index.css"
import { Header } from "./modules/Header"
import { Route, Routes } from "react-router-dom";
import { PublicPage } from "./pages/PublicPage";
import { LoginPage } from "./pages/LoginPage";
import { InvalidPage } from "./pages/InvalidPage";
import { RegisterPage } from "./pages/RegisterPage";
import { Paths } from "./helpers/enums";
function App() {
    return (
        <>
            <Header />

            <Routes>
                <Route path={Paths.homePath} element={<PublicPage />}></Route>
                <Route path={Paths.loginPath} element={<LoginPage />}></Route>
                <Route path={Paths.registerPath} element={<RegisterPage />}></Route>
                {/* <Route path="profile" element={<ProfilePage />}></Route> */}
                <Route path="*" element={<InvalidPage />}></Route>
            </Routes>
        </>
    );
}



export default App
