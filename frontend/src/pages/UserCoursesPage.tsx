import { MainContainer } from "../components/MainContainer";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { useAppSelector } from "../hooks/redux";
import { useEffect } from "react";
import { Paths } from "../models/paths";
import { useParams } from "react-router-dom";
import SelectionPanel from "../ui/SelectionPanel";
import SelectionItem from "../ui/SelectionItem";
import { ViewTypes } from "../models/enums";
import ControlNav from "../ui/ControlNav";
import UserCoursesModule from "../modules/UserCoursesModule";

function UserCoursesPage() {
    const { toNext } = useHistoryNavigation();
    const { isAuthenticated } = useAppSelector(state => state.authReducer);
    const param = useParams<'type'>();

    useEffect(() => {
        if (!isAuthenticated) toNext(Paths.homePath);

    }, [isAuthenticated, toNext]);


    return (
        <MainContainer>
            <div className="line-distributed-container">
                <h3>Мои курсы</h3>
                <ControlNav>
                    <button className="control-nav__add-button button-gray icon-plus"></button>
                </ControlNav>
            </div>

            <SelectionPanel>
                <SelectionItem
                    key={1}
                    className="selection-panel__selection-item"
                    title="Созданные"
                    active={param.type === ViewTypes.created}
                    onClick={() => toNext(Paths.userCoursesPath + '/' + ViewTypes.created)}
                />

                <SelectionItem
                    key={2}
                    className="selection-panel__selection-item"
                    title="Подписки"
                    active={param.type === ViewTypes.subscribed}
                    onClick={() => toNext(Paths.userCoursesPath + '/' + ViewTypes.subscribed)}
                />

                <SelectionItem
                    key={3}
                    className="selection-panel__selection-item"
                    title="Скрытые"
                    active={param.type === ViewTypes.unavailable}
                    onClick={() => toNext(Paths.userCoursesPath + '/' + ViewTypes.unavailable)}
                />
            </SelectionPanel>
            <UserCoursesModule type={param.type} />
        </MainContainer>
    );
}

export default UserCoursesPage;