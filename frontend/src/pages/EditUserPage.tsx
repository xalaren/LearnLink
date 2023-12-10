import { MainContainer } from "../components/MainContainer";
import { EditActions } from "../helpers/enums";

interface IEditUserPageProps {
    action: EditActions;
}

function EditUserPage({ action }: IEditUserPageProps) {
    return (
        <MainContainer title={action == EditActions.editUser ? 'Редактировать данные пользователя' : 'Изменить пароль'}>

        </MainContainer>
    )
}

export default EditUserPage;