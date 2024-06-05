import { Link } from "react-router-dom";
import { paths } from "../models/paths";

function Footer() {
    return (
        <footer className="footer">
            <div className="footer__content container">
                <p className="footer__title">
                    Learn Link
                </p>
                <p className="footer__text ui-text">
                    Платформа для разработки и публикации образовательных курсов
                    <p>©2023-2024</p>
                    <p><Link to={paths.privacy.full}>Политика конфиденциальности</Link></p>
                </p>
            </div>
        </footer>
    );
}

export default Footer;