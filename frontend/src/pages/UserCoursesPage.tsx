import { useParams } from "react-router-dom";
import { MainContainer } from "../components/MainContainer";
import SelectionPanel from "../ui/SelectionPanel";
import { Paths, ViewTypes } from "../models/enums";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import CreatedCourseContainer from "../modules/CreatedCoursesContainer";
import SubscribedCoursesContainer from "../modules/SubscribedCoursesContainer";
import { useAppSelector } from "../hooks/redux";
import { useEffect } from "react";
import UserCourseCreator from "../modules/UserCourseCreator";

function UserCoursesPage() {
    const param = useParams<'type'>();
    const { toNext } = useHistoryNavigation();
    const { isAuthenticated } = useAppSelector(state => state.authReducer);

    useEffect(() => {
        if (!isAuthenticated) toNext(Paths.homePath);
    }, [isAuthenticated, toNext]);

    return (
        <MainContainer>
            <UserCourseCreator />
            <SelectionPanel selectionItems={[
                {
                    title: "Созданные",
                    onClick: () => { toNext(`${Paths.userCoursesPath}/${ViewTypes.created}`) },
                    active: param.type === ViewTypes.created
                },
                {
                    title: "Подписки",
                    onClick: () => { toNext(`${Paths.userCoursesPath}/${ViewTypes.subscribed}`) },
                    active: param.type === ViewTypes.subscribed
                }]}
            />

            {param.type === ViewTypes.created && <CreatedCourseContainer />}
            {param.type === ViewTypes.subscribed && <SubscribedCoursesContainer />}
        </MainContainer>
    );
}

export default UserCoursesPage;