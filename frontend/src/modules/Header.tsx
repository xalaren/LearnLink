import { useNavigate } from "react-router-dom";
import { HeaderNav } from "../components/HeaderNav";


export function Header() {
    const navigate = useNavigate();

    return (
        <header className="header">
            <div className="container">
                <h1 className="header__title" onClick={() => navigate('/')}>Learn Link</h1>

                <HeaderNav />

            </div>
        </header>
    )
}