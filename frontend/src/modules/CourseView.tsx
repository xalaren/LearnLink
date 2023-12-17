import { useEffect, useState } from "react";
import { useGetCourse, useUserCourseStatus } from "../hooks/courseHooks";
import { ErrorModal } from "../components/ErrorModal";
import { Loader } from "../ui/Loader";
import { useAppSelector } from "../hooks/redux";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths } from "../models/enums";
import EllipsisDropdown from "../components/EllipsisDropdown";
import CourseInfo from "../components/CourseSidebar";
import CourseInfoSidebar from "./CourseInfoSidebar";
import ModulesContainer from "./ModulesContainer";

interface ICourseViewProps {
    courseId: number;
}

function CourseView({ courseId }: ICourseViewProps) {
    const { getCourseQuery, course, loading, error: courseError, resetValues: resetCourseValues } = useGetCourse();
    const { getStatusesQuery, error: statusError, resetError: resetStatusError, isCreator, isSubscriber } = useUserCourseStatus();
    const { toNext } = useHistoryNavigation();

    const { isAuthenticated, accessToken } = useAppSelector(state => state.authReducer);
    const user = useAppSelector(state => state.userReducer.user);

    const [localError, setLocalError] = useState('');
    const [isSubscriptionChanged, setSubscriptionChanged] = useState(false);

    useEffect(() => {
        fetchCourse();
    }, [courseId, user, accessToken]);

    useEffect(() => {
        if (isSubscriptionChanged) {
            fetchCourse();
            setSubscriptionChanged(false);
        }
    }, [isSubscriptionChanged]);

    useEffect(() => {
        if (courseError) setLocalError(courseError);
        if (statusError) setLocalError(statusError);
    }, [courseError, localError, statusError])

    async function fetchCourse() {
        if (courseId !== 0) {
            await getCourseQuery(courseId, user?.id);
            if (user && accessToken) await getStatusesQuery(user.id!, courseId, accessToken);
        }
    }

    function resetError() {
        resetCourseValues();
        resetStatusError();
        setLocalError('');
    }

    return (
        <>
            {loading && <Loader />}

            {!localError && !loading && course &&
                <section className="course-view">
                    <div className="course-view__header">
                        <h2 className="course-view__title medium-big">{course.title}</h2>
                    </div>
                    <div className="course-view__description">
                        <p className="description regular">
                            {course.description}
                        </p>
                    </div>

                    <div className="course-view__content">
                        <ModulesContainer />
                    </div>

                    <CourseInfoSidebar course={course} isSubscriber={isSubscriber} isCreator={isCreator} subscriptionChanged={() => setSubscriptionChanged(true)} />


                    {isCreator && <p>You are creator</p>}
                    {!isAuthenticated && <p>You need to register or login</p>}
                    {!isCreator && !isSubscriber && <p>You are not a subscriber</p>}
                    {!isCreator && isSubscriber && <p>You are subscriber</p>}
                </section >
            }

            {localError && <ErrorModal active={Boolean(localError)} onClose={() => {
                resetError();
                toNext(Paths.homePath);
            }} error={localError} />}
        </>
    );
}

export default CourseView;