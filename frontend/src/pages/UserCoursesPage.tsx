import { useParams } from "react-router-dom";
import { MainContainer } from "../components/MainContainer";
import SelectionPanel from "../ui/SelectionPanel";
import { Paths, ViewTypes } from "../models/enums";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import CreatedCourseContainer from "../modules/CreatedCoursesContainer";
import SubscribedCoursesContainer from "../modules/SubscribedCoursesContainer";

function UserCoursesPage() {
    const param = useParams<'type'>();
    const { toNext } = useHistoryNavigation();

    return (
        <MainContainer title="Мои доступные курсы">
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