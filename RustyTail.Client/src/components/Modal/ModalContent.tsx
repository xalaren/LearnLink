interface IModalContentProps {
    children: React.ReactNode;
    className?: string;
}

function ModalContent({ children, className = '' }: IModalContentProps) {
    return (
        <div className={"modal__body ui-text " + className}>
            {children}
        </div>
    );
}

export default ModalContent;