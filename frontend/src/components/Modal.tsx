interface IModalProps {
    children?: React.ReactNode;
    title: string;
    contentClassName?: string;
    active: boolean;
    onClose: () => void;
}
export function Modal({ active, title, contentClassName, children, onClose }: IModalProps) {
    contentClassName = "modal__content " + (contentClassName ? contentClassName : '');

    if (!active) { return null }

    return (
        <div className="modal">

            <div className="modal__container">

                <div className="modal__head">
                    <h3 className="modal__title">{title}</h3>
                    <button className="modal__close-button" onClick={onClose}>
                        ×
                    </button>
                </div>

                <div className={contentClassName}>
                    {children}
                </div>
            </div>
        </div >
    )
}