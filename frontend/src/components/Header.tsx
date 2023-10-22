import React, {useState} from "react";
export function Header(props: any) {
    const [isAuthorized, setAuthorized] = useState(false);

    return(
        <header className="header">
            <div className="container">
                <h1 className="header__title">Платформа курсов</h1>

                {isAuthorized &&
                    <nav className="header__nav">
                        <a className="white-link">Мои курсы</a>
                        <a className="white-link">Личный кабинет</a>
                        <a className="white-link">Выйти</a>
                    </nav>
                }

                {!isAuthorized &&
                    <nav className="header__nav">
                        <button className="white-tp-button">Регистрация</button>
                        <button className="white-tp-button">Войти</button>
                    </nav>
                }
            </div>
        </header>
    )
}