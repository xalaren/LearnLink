import "./styles/css/index.css"
import { Header } from "./modules/Header"
import { InvalidPathComponent } from "./components/InvalidPathComponent"
import { Route, Routes } from "react-router-dom";
import { PublicPage } from "./pages/PublicPage";

function App() {
    return (
        <>
            <Header />

            <Routes>
                <Route path="/" element={<PublicPage />}></Route>
                {/* <Route path="login" element={<LoginPage />}></Route>
                    <Route path="register" element={<RegisterPage />}></Route>
                    <Route path="profile" element={<ProfilePage />}></Route> */}
                <Route path="*" element={<InvalidPathComponent />}></Route>
            </Routes>
        </>
    );
}



export default App
