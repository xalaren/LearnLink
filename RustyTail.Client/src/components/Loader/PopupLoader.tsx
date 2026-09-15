import { Loader } from "./Loader";

function PopupLoader() {
    return (
        <div className="popup-loader">

            <div className="popup-loader__container">
                <Loader />
            </div>
        </div >);
}

export default PopupLoader;