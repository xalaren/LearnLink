interface IModalButtonProps {
    text: string;
    onClick: () => void;
    className?: string;
}

function ModalButton({ text, onClick, className }: IModalButtonProps) {
    const buttonClass: string = className || 'button-violet-light';

    return (
        <button className={"modal__bottom-button " + buttonClass} onClick={onClick}>{text}</button>
    )
}

export default ModalButton;