import { useNavigate } from "react-router-dom";

interface IHeaderProps {
    children: React.ReactNode;
}

export function Header({ children }: IHeaderProps) {
    const navigate = useNavigate();

    return (
        <header className="header">
            <div className="container">
                <h1 className="header__title" onClick={() => navigate('/')}>Платформа курсов</h1>

                {children}

            </div>
        </header>
    )
}