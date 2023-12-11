import { MainContainer } from "../components/MainContainer";
import PublicCoursesContainer from "../modules/PublicCoursesContainer";

export function PublicPage() {
    return (
        <MainContainer title="Общедоступные курсы">
            <PublicCoursesContainer />
        </MainContainer>
    )
}