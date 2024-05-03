import { useParams } from "react-router-dom";
import { MainContainer } from "../components/MainContainer";
import { useEffect, useState } from "react";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { useAppSelector } from "../hooks/redux";
import { useGetCourse, useGetCreatorStatus, useGetSubscriberStatus } from "../hooks/courseHooks";
import { Course } from "../models/course";
import { ErrorModal } from "../components/Modal/ErrorModal";
import { Loader } from "../components/Loader/Loader";
import CourseView from "../modules/CourseView";
import { useSubscription } from "../hooks/subscriptionHooks";
function CoursePage() {
    const param = useParams<'courseId'>();
    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { toPrev } = useHistoryNavigation();

    const [course, setCourse] = useState<Course>();
    const [subscriberStatus, setSubscriberStatus] = useState(false);
    const [creatorStatus, setCreatorStatus] = useState(false);

    const courseHook = useGetCourse();
    const subscribeHook = useSubscription();
    const getSubscriberStatusHook = useGetSubscriberStatus();
    const getCreatorStatusHook = useGetCreatorStatus();

    useEffect(() => {
        fetchData();
    }, [user])


    async function fetchData() {
        await fetchUserStatus();
        await fetchCourse();
    }

    async function fetchUserStatus() {
        if (!user || !accessToken) {
            return;
        }

        const isSubscriber = await getSubscriberStatusHook.getStatusesQuery(user.id, Number(param.courseId), accessToken);
        const isCreator = await getCreatorStatusHook.getStatusesQuery(user.id, Number(param.courseId), accessToken);

        if (isCreator != undefined) {
            setCreatorStatus(isCreator);
        }

        if (isSubscriber != undefined) {
            setSubscriberStatus(isSubscriber);
        }
    }

    async function fetchCourse() {
        if (!user) {
            return;
        }

        const foundCourse = await courseHook.getCourseQuery(Number(param.courseId), user.id);

        if (foundCourse) {
            setCourse(foundCourse);
        }
    }

    async function onSubscribe() {
        if (user && accessToken) {
            await subscribeHook.subscribeQuery(user.id, Number(param.courseId), accessToken);
        }

        await fetchData();
    }

    async function onUnsubscribe() {
        if (user && accessToken) {
            await subscribeHook.unsubscribeQuery(user.id, Number(param.courseId), accessToken);
        }

        await fetchData();
    }

    function onCourseLoadingError() {
        courseHook.resetValues();
        toPrev();
    }

    return (
        <MainContainer className="view-page">
            {course ?
                <BuildedPage
                    courseError={courseHook.error}
                    courseLoading={courseHook.loading}
                    onCourseError={onCourseLoadingError}>

                    <CourseView
                        course={course}
                        isSubscriber={subscriberStatus}
                        isCreator={creatorStatus}
                        onSubscribe={onSubscribe}
                        onUnsubscribe={onUnsubscribe} />

                </BuildedPage>
                :
                <p>Ошибка загрузки курса...</p>
            }
        </MainContainer>
    );
}


function BuildedPage(props: {
    courseLoading: boolean,
    courseError: string,
    onCourseError: () => void,
    children: React.ReactNode
}) {

    if (props.courseLoading) {
        return (<Loader />);
    }

    if (props.courseError) {
        return (<ErrorModal active={Boolean(props.courseError)} onClose={props.onCourseError} error={props.courseError} />)
    }

    return props.children;
}




export default CoursePage;