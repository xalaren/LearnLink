import { Route, Routes } from 'react-router-dom'
import { Header } from "./components/Header.tsx";
import { PublicPage } from "./pages/PublicPage.tsx";
import { LoginPage } from './pages/LoginPage.tsx';
import { RegisterPage } from './pages/RegisterPage.tsx';
import { HeaderNav } from './components/HeaderNav.tsx';
import { ProfilePage } from "./pages/ProfilePage.tsx";
import "./css/index.css"
import { AuthorizationState } from "./context/AuthorizationContext.tsx";
import { EditUserPage } from "./pages/EditUserPage.tsx";
import { NoMatch } from "./components/NoMatch.tsx";
import { CoursesPage, ViewType } from "./pages/CoursesPage.tsx";

function App() {
    return (
        <AuthorizationState>
            <Header>
                <HeaderNav />
            </Header>

            <Routes>
                <Route path="/" element={<PublicPage />} />
                <Route path="login" element={<LoginPage />}></Route>
                <Route path="register" element={<RegisterPage />}></Route>
                <Route path="profile" element={<ProfilePage />}></Route>
                {/* <Route path="profile/edit" element={<EditUserPage action={EditActions.EditUser} />}></Route>
                <Route path="profile/edit/password" element={<EditUserPage action={EditActions.EditPassword} />}></Route> */}
                <Route path="*" element={<NoMatch />} />

            </Routes>
        </AuthorizationState>
    )
}



export default App
