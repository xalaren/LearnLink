import { useEffect } from "react";
import { MainContainer } from "../components/MainContainer";
import { useAppSelector } from "../hooks/redux";
import { Paths } from "../models/paths";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import SideNav from "../components/SideNav/SideNav";
import SideNavLink from "../components/SideNav/SideNavLink";
import { useParams } from "react-router-dom";
import { ProfileEditActions } from "../models/enums";
import EditProfileMainModule from "../modules/EditProfileModules/EditProfileMainModule.tsx";
import EditProfilePasswordModule from "../modules/EditProfileModules/EditProfilePasswordModule.tsx";
import ProfileDeleteModule from "../modules/EditProfileModules/ProfileDeleteModule.tsx";

function ProfilePage() {
    const { isAuthenticated } = useAppSelector(state => state.authReducer);
    const { toNext } = useHistoryNavigation();
    const param = useParams<'action'>();

    useEffect(() => {
        if (!isAuthenticated) {
            toNext(Paths.loginPath);
        }
    }, [isAuthenticated])

    return (
        <MainContainer className="account-page">
            <SideNav className="account-page__nav">
                <SideNavLink
                    selected={param.action === ProfileEditActions.main}
                    text="Основные данные"
                    iconClassName="icon-user"
                    onClick={() => toNext(Paths.editProfileMainPath)}
                />
                <SideNavLink
                    selected={param.action === ProfileEditActions.password}
                    text="Изменить пароль"
                    iconClassName="icon-lock"
                    onClick={() => toNext(Paths.editProfilePasswordPath)}
                />

                <SideNavLink
                    selected={param.action === ProfileEditActions.delete}
                    text="Удалить профиль"
                    iconClassName="icon-cross"
                    onClick={() => toNext(Paths.deleteProfilePath)}
                />
            </SideNav>

            <div className="account-page__content">

                {param.action === ProfileEditActions.main &&
                    <EditProfileMainModule />
                }

                {param.action === ProfileEditActions.password &&
                    <EditProfilePasswordModule />
                }

                {param.action === ProfileEditActions.delete &&
                    <ProfileDeleteModule />
                }
            </div>

        </MainContainer>
    );
}

export default ProfilePage;