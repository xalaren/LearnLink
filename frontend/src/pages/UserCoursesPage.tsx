import { useParams } from "react-router-dom";
import { MainContainer } from "../components/MainContainer";
import { Paths, ViewTypes } from "../models/enums";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { useAppSelector } from "../hooks/redux";
import { useEffect, useState } from "react";
import UserCourseCreator from "../modules/UserCourseCreator";

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