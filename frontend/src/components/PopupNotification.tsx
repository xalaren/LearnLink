import { useEffect } from "react";
import { NotificationType } from "../models/enums";
import success from "../assets/img/success.svg";
import failed from "../assets/img/failed.svg";

interface IPopupNotificationProps {
    notificationType: NotificationType;
    children: string;
    onFade: () => void;
}

function PopupNotification({ notificationType, children, onFade }: IPopupNotificationProps) {
    const className = notificationType === NotificationType.default ? '' : `notification-${notificationType}`;

    useEffect(() => {
        setTimeout(() => {
            onFade();
        }, 2000);
    }, [onFade]);

    return (
        <div className={`notification ${className}`}>
            {notificationType === NotificationType.success &&
                <img className="notification__icon" src={success}></img>
            }

            {notificationType === NotificationType.error &&
                <img className="notification__icon" src={failed}></img>
            }

            {children}
        </div>
    );
}

export default PopupNotification;