import { useNavigate } from "react-router-dom";
import { MainContainer } from "../components/MainContainer";
import { EditActions, Paths } from "../models/enums";
import { useAppSelector } from "../hooks/redux";
import { EditUserForm } from "../modules/EditUserForm";
import { useEffect } from "react";
import EditPassForm from "../modules/EditPassForm";

interface IEditUserPageProps {
    action: EditActions;
}

function EditUserPage({ action }: IEditUserPageProps) {
    const { isAuthenticated } = useAppSelector(state => state.authReducer);
    const navigate = useNavigate();

    useEffect(() => {
        if (!isAuthenticated) navigate(Paths.loginPath);
    }, [isAuthenticated, navigate]);

    return (
        <MainContainer title={action == EditActions.editUser ? 'Редактировать данные пользователя' : 'Изменить пароль'}>
            {action == EditActions.editUser && <EditUserForm />}
            {action == EditActions.editPassword && <EditPassForm />}
        </MainContainer>
    )
}

export default EditUserPage;