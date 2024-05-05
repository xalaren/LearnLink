import { useEffect, useState } from "react";
import SearchForm from "../components/SearchForm";
import { Course } from "../models/course";
import { useAppSelector } from "../hooks/redux";
import { Loader } from "../components/Loader/Loader";
import { ErrorModal } from "../components/Modal/ErrorModal";
import { CoursesContainer } from "../components/CoursesContainer/CoursesContainer";
import Paginate from "../components/Paginate";
import { ViewTypes } from "../models/enums";
import { Paths } from "../models/paths";
import { useParams } from "react-router-dom";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { useUserCourses } from "../hooks/courseHooks";

interface IUserCoursesModuleProps {
    type?: string;
    shouldUpdate: any;
}

function UserCoursesModule({ type, shouldUpdate }: IUserCoursesModuleProps) {
    const param = useParams<'pageNumber'>();

    const [page, setPage] = useState(Number(param.pageNumber));
    const [pageCount, setPageCount] = useState(1);
    const [searchText, setSearchText] = useState('');
    const [courses, setCourses] = useState<Course[]>();
    const { accessToken } = useAppSelector(state => state.authReducer);
    const { user } = useAppSelector(state => state.userReducer);
    const { toNext } = useHistoryNavigation();

    const { userCoursesQuery, loading, error, resetValues } = useUserCourses();

    useEffect(() => {
        navigateToPage(1);
    }, [type])

    useEffect(() => {
        fetchCourses();
    }, [user, param, shouldUpdate]);

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;
        setSearchText(inputElement.value);
    }

    async function onSubmit(event: React.FormEvent) {
        event.preventDefault();
        navigateToPage(1);
    }

    async function fetchCourses() {
        resetValues();

        if (user && accessToken) {
            const isSubscription = type === ViewTypes.subscribed;
            const isUnavailable = type === ViewTypes.unavailable;
            const dataPage = await userCoursesQuery(user.id, accessToken, searchText, isSubscription, isUnavailable, page);

            if (dataPage) {
                setCourses(dataPage.values);
                setPageCount(dataPage.pageCount);
            }
            else {
                setPageCount(1);
                setCourses(undefined);
            }
        }
    }

    function navigateToPage(nextPage: number) {
        setPage(nextPage);
        toNext(`${Paths.userCoursesPath}/${type}/page/${nextPage}`);
    }
    return (
        <>
            <SearchForm placeholder="Название курсов" value={searchText} onChange={onChange} onSubmit={onSubmit} />

            <Paginate currentPage={page} pageCount={pageCount} setPage={navigateToPage} />

            <ResultCourseContainer
                loading={loading}
                errorText={error}
                courses={courses}
                onModalClose={resetValues}
            />
        </>
    );
}

interface IResultCourseContainerProps {
    loading: boolean;
    errorText?: string;
    courses?: Course[];
    onModalClose: () => void;
}

function ResultCourseContainer(props: IResultCourseContainerProps) {

    if (props.loading) return (
        <Loader />
    )

    if (props.errorText) return (
        <ErrorModal
            active={Boolean(props.errorText)}
            error={props.errorText}
            onClose={props.onModalClose}
        />
    )

    if (props.courses) {
        return (
            <CoursesContainer courses={props.courses} />
        )
    }
}

export default UserCoursesModule;