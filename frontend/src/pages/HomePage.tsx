import { MainContainer } from "../components/MainContainer";
import student from "../assets/img/student.png"
import { Link } from "react-router-dom";
import { paths } from "../models/paths";

function HomePage() {

    return (
        <MainContainer>
            <div className="greetings">
                <div className="greetings__info ui-text">
                    <h1>
                        Добро пожаловать в образовательную платформу <span className="violet-selection">Learn Link</span>
                    </h1>
                    <p className="greetings__description">
                        Мы поможем сделать образование более доступным и адаптивным <br /><br />
                        Для этого мы предлагаем Вам простые и удобные инструменты для создания своих образовательных материалов
                    </p>
                    <div className="greetings__links">
                        <Link className="greetings__link" to={paths.public()}>
                            <p>Перейти к курсам</p>
                            <span className="icon icon-arrow-right"></span>
                        </Link>

                    </div>
                </div>
                <div className="greetings__image">
                    <img src={student}></img>
                </div>
            </div>
        </MainContainer>
    );
}

export default HomePage;