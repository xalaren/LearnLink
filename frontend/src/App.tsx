import { Route, Routes } from 'react-router-dom'
import { Header } from "./components/Header.tsx";
import { PublicPage } from "./components/PublicPage.tsx";
import { LoginPage } from './components/LoginPage.tsx';
import { RegisterPage } from './components/RegisterPage.tsx';
import { HeaderNav } from './components/HeaderNav.tsx';
import {ProfilePage} from "./components/ProfilePage.tsx";
import "./css/index.css"
import {AuthorizationState} from "./context/AuthorizationContext.tsx";

function App() {
    return (
        <AuthorizationState>
            <Header>
                <HeaderNav />
            </Header>

            <Routes>
                <Route path="/" element={<PublicPage />} />
                <Route path="/login" element={<LoginPage />}></Route>
                <Route path="/register" element={<RegisterPage />}></Route>
                <Route path="/profile" element={<ProfilePage />}></Route>
            </Routes>
        </AuthorizationState>
    )
}



export default App
