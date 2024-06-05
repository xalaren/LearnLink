import { MainContainer } from "../components/MainContainer";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { useAppSelector } from "../hooks/redux";
import { useEffect, useState } from "react";
import { paths } from "../models/paths";
import { useParams } from "react-router-dom";
import SelectionPanel from "../components/Selection/SelectionPanel";
import SelectionItem from "../components/Selection/SelectionItem";
import { ViewTypes } from "../models/enums";
import ControlNav from "../components/ControlNav";
import UserCoursesModule from "../modules/UserCoursesModule";
import Breadcrumb from "../components/Breadcrumb/Breadcrumb";
import BreadcrumbItem from "../components/Breadcrumb/BreadcrumbItem";
import CourseCreateModal from "../modules/Courses/CourseCreateModal";

function UserCoursesPage() {
    const { toNext } = useHistoryNavigation();
    const { isAuthenticated } = useAppSelector(state => state.authReducer);
    const param = useParams<'type'>();
    const [createModalActive, setCreateModalActive] = useState(false);


    useEffect(() => {
        if (!isAuthenticated) toNext(paths.public());

    }, [isAuthenticated, toNext])


    return (
        <MainContainer>

            <Breadcrumb>
                <BreadcrumbItem text="В начало" path={paths.public()} />
                <BreadcrumbItem text="Мои курсы" path={paths.profile.courses(ViewTypes.created)} />
                {param.type === ViewTypes.created &&
                    <BreadcrumbItem text="Созданные" />
                }
                {param.type === ViewTypes.subscribed &&
                    <BreadcrumbItem text="Подписки" />
                }
                {param.type === ViewTypes.unavailable &&
                    <BreadcrumbItem text="Скрытые" />
                }
            </Breadcrumb>

            <div className="line-distributed-container">
                <h3>Мои курсы</h3>
                <ControlNav>
                    <button className="control-nav__add-button button-gray icon icon-medium-size icon-plus" onClick={() => setCreateModalActive(true)}></button>
                </ControlNav>
            </div>

            <SelectionPanel>
                <SelectionItem
                    index={1}
                    className="selection-panel__selection-item"
                    title="Созданные"
                    active={param.type === ViewTypes.created}
                    onClick={() => toNext(paths.profile.courses(ViewTypes.created))}
                />

                <SelectionItem
                    index={2}
                    className="selection-panel__selection-item"
                    title="Подписки"
                    active={param.type === ViewTypes.subscribed}
                    onClick={() => toNext(paths.profile.courses(ViewTypes.subscribed))}
                />

                <SelectionItem
                    index={3}
                    className="selection-panel__selection-item"
                    title="Скрытые"
                    active={param.type === ViewTypes.unavailable}
                    onClick={() => toNext(paths.profile.courses(ViewTypes.unavailable))}
                />
            </SelectionPanel>

            <UserCoursesModule type={param.type || ViewTypes.created} shouldUpdate={createModalActive} />

            <CourseCreateModal active={createModalActive} onClose={() => setCreateModalActive(false)} />
        </MainContainer>
    );
}

export default UserCoursesPage;