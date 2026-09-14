interface IModalFooterProps {
    children: React.ReactNode;
}

function ModalFooter({ children }: IModalFooterProps) {
    return (
        <div className="modal__footer">
            {children}
        </div>
    )
}

export default ModalFooter;