import { MainContainer } from "../components/MainContainer";
import { EditActions } from "../helpers/enums";
import { EditUserForm } from "../modules/EditUserForm";

interface IEditUserPageProps {
    action: EditActions;
}

function EditUserPage({ action }: IEditUserPageProps) {
    return (
        <MainContainer title={action == EditActions.editUser ? 'Редактировать данные пользователя' : 'Изменить пароль'}>
            {action == EditActions.editUser && <EditUserForm />}
        </MainContainer>
    )
}

export default EditUserPage;