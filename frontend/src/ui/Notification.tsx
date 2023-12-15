import { useEffect } from "react";
import { NotificationType } from "../models/enums";

interface INotificationProps {
    notificationType: NotificationType;
    children: string;
    onFade: () => void;
}

function Notification({ notificationType, children, onFade }: INotificationProps) {
    const className = notificationType === NotificationType.default ? 'notification' : `notification-${notificationType}`;

    useEffect(() => {
        setTimeout(() => {
            onFade();
        }, 650);
    }, [onFade]);

    return (
        <div className={className}>
            {children}
        </div>
    );
}

export default Notification;