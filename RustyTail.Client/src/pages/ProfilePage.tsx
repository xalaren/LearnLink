import { useEffect } from "react";
import { MainContainer } from "../components/MainContainer";
import { useAppSelector } from "../hooks/redux";
import { paths } from "../models/paths";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import SideNav from "../components/SideNav/SideNav";
import SideNavLink from "../components/SideNav/SideNavLink";
import { useParams } from "react-router-dom";
import { ProfileEditActions } from "../models/enums";
import EditProfileMainModule from "../modules/EditProfileModules/EditProfileMainModule.tsx";
import EditProfilePasswordModule from "../modules/EditProfileModules/EditProfilePasswordModule.tsx";
import ProfileDeleteModule from "../modules/EditProfileModules/ProfileDeleteModule.tsx";
import Breadcrumb from "../components/Breadcrumb/Breadcrumb.tsx";
import BreadcrumbItem from "../components/Breadcrumb/BreadcrumbItem.tsx";

function ProfilePage() {
    const { isAuthenticated } = useAppSelector(state => state.authReducer);
    const { toNext } = useHistoryNavigation();
    const param = useParams<'action'>();

    useEffect(() => {
        if (!isAuthenticated) {
            toNext(paths.login);
        }
    }, [isAuthenticated])

    return (
        <MainContainer>

            <Breadcrumb>
                <BreadcrumbItem text="В начало" path={paths.public()} />
                <BreadcrumbItem text="Профиль" path={paths.profile.edit(ProfileEditActions.main)} />
                {param.action === ProfileEditActions.main &&
                    <BreadcrumbItem text="Основные данные" />
                }
                {param.action === ProfileEditActions.password &&
                    <BreadcrumbItem text="Изменить пароль" />
                }
                {param.action === ProfileEditActions.delete &&
                    <BreadcrumbItem text="Удалить данные" />
                }
            </Breadcrumb>

            <section className="account-page">
                <SideNav className="account-page__nav">
                    <SideNavLink
                        selected={param.action === ProfileEditActions.main}
                        text="Основные данные"
                        iconClassName="icon-user"
                        onClick={() => toNext(paths.profile.edit(ProfileEditActions.main))}
                    />
                    <SideNavLink
                        selected={param.action === ProfileEditActions.password}
                        text="Изменить пароль"
                        iconClassName="icon-lock"
                        onClick={() => toNext(paths.profile.edit(ProfileEditActions.password))}
                    />

                    <SideNavLink
                        selected={param.action === ProfileEditActions.delete}
                        text="Удалить профиль"
                        iconClassName="icon-cross"
                        onClick={() => toNext(paths.profile.edit(ProfileEditActions.delete))}
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
            </section>

        </MainContainer>
    );
}

export default ProfilePage;