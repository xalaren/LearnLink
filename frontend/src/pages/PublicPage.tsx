import { CoursesContainer } from "../components/CoursesContainer";
import { MainContainer } from "../components/MainContainer";
import SearchForm from "../components/SearchForm";
import { Course } from "../models/course";

export function PublicPage() {
    return (
        <MainContainer title="Общедоступные курсы">
            <SearchForm />

            <CoursesContainer courses={[
                new Course(
                    0,
                    "Title 0",
                    true,
                    false,
                    "description",
                    10000,
                ),
                new Course(
                    1,
                    "Title 1",
                    false,
                    false,
                    "description",
                    20000,
                )
            ]} />
        </MainContainer>
    )
}