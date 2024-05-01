import { ErrorText } from "./ErrorText";
import { Modal } from "./Modal";

interface IErrorModalProps {
    active: boolean;
    onClose: () => void;
    error: string;
}


export function ErrorModal({ active, onClose, error }: IErrorModalProps) {
    return (
        <Modal title="Ошибка" active={active} onClose={onClose}>
            <ErrorText>{error}</ErrorText>
        </Modal>
    )
}