import { MainContainer } from "../components/MainContainer";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { useAppSelector } from "../hooks/redux";
import { useEffect } from "react";
import UserCourseCreator from "../modules/UserCourseCreator";
import { Paths } from "../models/paths";

function UserCoursesPage() {
    const { toNext } = useHistoryNavigation();
    const { isAuthenticated } = useAppSelector(state => state.authReducer);

    useEffect(() => {
        if (!isAuthenticated) toNext(Paths.homePath);

    }, [isAuthenticated, toNext]);


    return (
        <MainContainer>
            <UserCourseCreator />
        </MainContainer>
    );
}

export default UserCoursesPage;