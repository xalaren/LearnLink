import { Outlet, useParams } from "react-router-dom";
import { MainContainer } from "../components/MainContainer";
import { useContext, useEffect, useState } from "react";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { useAppSelector } from "../hooks/redux";
import { useGetCourse, useGetCreatorStatus, useGetSubscriberStatus } from "../hooks/courseHooks";
import { Course } from "../models/course";
import { ErrorModal } from "../components/Modal/ErrorModal";
import { Loader } from "../components/Loader/Loader";
import CourseView from "../modules/Courses/CourseView";
import { useSubscription } from "../hooks/subscriptionHooks";
import BreadcrumbContainer from "../components/Breadcrumb/Breadcrumb";
import BreadcrumbItem from "../components/Breadcrumb/BreadcrumbItem";
import { paths } from "../models/paths";
import { ViewTypes } from "../models/enums";
import { CourseContext } from "../contexts/CourseContext";
function CoursePage() {
    const param = useParams<'courseId'>();

    const { user } = useAppSelector(state => state.userReducer);
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { setCourseContextData } = useContext(CourseContext);

    const { toPrev } = useHistoryNavigation();

    const [course, setCourse] = useState<Course>();
    const [subscriberStatus, setSubscriberStatus] = useState(false);
    const [creatorStatus, setCreatorStatus] = useState(false);

    const courseHook = useGetCourse();
    const subscribeHook = useSubscription();
    const getSubscriberStatusHook = useGetSubscriberStatus();
    const getCreatorStatusHook = useGetCreatorStatus();

    const [updateModalActive, setUpdateModalActive] = useState(false);
    const [deleteModalActive, setDeleteModalActive] = useState(false);

    useEffect(() => {
        if (updateModalActive || deleteModalActive) {
            return;
        }
        fetchData();
    }, [user, updateModalActive, deleteModalActive])


    async function fetchData() {
        await fetchUserStatus();
        await fetchCourse();
    }

    async function fetchUserStatus() {
        if (!user || !accessToken || !course) {
            return;
        }

        const isSubscriber = await getSubscriberStatusHook.getStatusesQuery(user.id, course.id, accessToken);
        const isCreator = await getCreatorStatusHook.getStatusesQuery(user.id, course.id, accessToken);

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
            setCourseContextData(foundCourse);
        }
    }

    async function onSubscribe() {
        if (user && accessToken && course) {
            await subscribeHook.subscribeQuery(user.id, course.id, accessToken);
        }

        await fetchData();
    }

    async function onUnsubscribe() {
        if (user && accessToken && course) {
            await subscribeHook.unsubscribeQuery(user.id, course.id, accessToken);
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
                <>

                    <BreadcrumbContainer>
                        <BreadcrumbItem text="В начало" path={paths.public()} />
                        {!course.isPublic &&
                            <BreadcrumbItem text="Мои курсы" path={paths.profile.courses(ViewTypes.created)} />
                        }
                        <BreadcrumbItem text={course.title} path={paths.course.view(course.id)} />
                    </BreadcrumbContainer>
                    <BuildedPage
                        courseError={courseHook.error}
                        courseLoading={courseHook.loading}
                        onCourseError={onCourseLoadingError}>

                        <CourseView
                            course={course}
                            isSubscriber={subscriberStatus}
                            isCreator={creatorStatus}
                            onSubscribe={onSubscribe}
                            onUnsubscribe={onUnsubscribe}
                            updateModalActive={updateModalActive}
                            setUpdateModalActive={setUpdateModalActive}
                            deleteModalActive={deleteModalActive}
                            setDeleteModalActive={setDeleteModalActive}
                        />
                    </BuildedPage>
                </>
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