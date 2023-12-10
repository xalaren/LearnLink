import "./styles/css/index.css"
import { Header } from "./modules/Header"
import { Route, Routes } from "react-router-dom";
import { PublicPage } from "./pages/PublicPage";
import { LoginPage } from "./pages/LoginPage";
import { InvalidPage } from "./pages/InvalidPage";
function App() {
    return (
        <>
            <Header />

            <Routes>
                <Route path="/" element={<PublicPage />}></Route>
                <Route path="login" element={<LoginPage />}></Route>
                {/*<Route path="register" element={<RegisterPage />}></Route>
                    <Route path="profile" element={<ProfilePage />}></Route> */}
                <Route path="*" element={<InvalidPage />}></Route>
            </Routes>
        </>
    );
}



export default App
