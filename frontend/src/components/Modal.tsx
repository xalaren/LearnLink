interface IModalButton {
    text: string;
    className?: string;
    onClick: () => void;
}

interface IModalProps {
    children?: React.ReactNode;
    title: string;
    contentClassName?: string;
    active: boolean;
    onClose: () => void;
    buttons?: IModalButton[];
}
export function Modal({ active, title, children, onClose, contentClassName = "", buttons }: IModalProps) {

    if (!active) { return null }

    return (
        <div className="modal">
            <div className="modal__content">
                <div className="modal__header">
                    <p className="modal__title ui-title">{title}</p>
                    <button className="modal__close-button icon-cross icon-normal-size" onClick={onClose}></button>
                </div>

                <div className={"modal__body ui-text " + contentClassName}>
                    {children}
                </div>

                {buttons &&
                    <ModalFooter buttons={buttons} />
                }
            </div>
        </div>
    )
}


function ModalFooter(props: { buttons: IModalButton[] }) {
    return (
        <div className="modal__footer">
            {props.buttons.map(button => {
                let buttonClass: string = button.className || 'button-violet-light';
                let key = props.buttons.indexOf(button);
                return (
                    <button className={"modal__bottom-button " + buttonClass} key={key} onClick={button.onClick}>{button.text}</button>
                )
            })}
        </div>
    )
}