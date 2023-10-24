import { Route, Routes } from 'react-router-dom'
import { Header } from "./components/Header.tsx";
import { PublicPage } from "./components/PublicPage.tsx";
import { LoginPage } from './components/LoginPage.tsx';
import { RegisterPage } from './components/RegisterPage.tsx';
import "./css/index.css"
import { HeaderNav } from './components/HeaderNav.tsx';

function App() {

    return (
        <>
            <Header>
                <HeaderNav />
            </Header>

            <Routes>
                <Route path="/" element={<PublicPage />} />
                <Route path="/login" element={<LoginPage />}></Route>
                <Route path="/register" element={<RegisterPage />}></Route>
            </Routes>
        </>
    )
}



export default App
