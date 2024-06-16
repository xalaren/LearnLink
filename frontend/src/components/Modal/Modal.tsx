import { useEffect } from "react";

interface IModalProps {
    children?: React.ReactNode;
    title: string;
    active: boolean;
    onClose: () => void;
    contentClassName?: string;
}

export function Modal({ active, title, children, onClose, contentClassName = '' }: IModalProps) {
    useEffect(() => {
        addKeyboardEventListeners();
    }, []);

    function addKeyboardEventListeners() {
        document.addEventListener('keydown', event => {
            if (event.key === "Escape") onClose();
        })
    }

    useEffect(() => {
        if (active) {
            document.body.style.overflow = 'hidden';
        } else {
            document.body.style.overflow = 'auto';
        }

        return () => {
            document.body.style.overflow = 'auto';
        };
    }, [active]);

    return (
        <>
            {active &&
                <div className="modal">
                    <div className={`modal__content ${contentClassName}`}>
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