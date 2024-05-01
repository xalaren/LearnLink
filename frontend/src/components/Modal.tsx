interface IModalProps {
    children?: React.ReactNode;
    title: string;
    active: boolean;
    onClose: () => void;
}

export function Modal({ active, title, children, onClose }: IModalProps) {
    return (
        <>
            {active &&
                <div className="modal">
                    <div className="modal__content">
                        <div className="modal__header">
                            <p className="modal__title ui-title">{title}</p>
                            <button className="modal__close-button icon-cross icon-normal-size" onClick={onClose}></button>
                        </div>
                        {children}
                    </div>
                </div>
            }
        </>
    )
}