import { Modal } from "./Modal";

interface ISuccessModalProps {
    active: boolean;
    onClose: () => void;
    message: string;
}


export function SuccessModal({ active, onClose, message }: ISuccessModalProps) {
    return (
        <Modal title="Успешно" active={active} onClose={onClose}>
            {message}
        </Modal>
    )
}