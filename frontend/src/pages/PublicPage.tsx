import { useEffect, useState } from "react";
import { CoursesContainer } from "../components/CoursesContainer/CoursesContainer";
import { MainContainer } from "../components/MainContainer";
import SearchForm from "../components/SearchForm";
import { Course } from "../models/course";
import { usePublicCourses } from "../hooks/courseHooks";
import { Loader } from "../components/Loader/Loader";
import { ErrorModal } from "../components/Modal/ErrorModal";
import Paginate from "../components/Paginate";

export function PublicPage() {
    const [page, setPage] = useState(1);
    const [pageCount, setPageCount] = useState(1);
    const [searchText, setSearchText] = useState('');
    const [courses, setCourses] = useState<Course[]>();


    const { publicCoursesQuery, loading, error, resetValues } = usePublicCourses();

    useEffect(() => {
        fetchCourses();
    }, [page])

    async function fetchCourses() {
        resetValues();

        const dataPage = await publicCoursesQuery(searchText, page);

        if (dataPage) {
            setCourses(dataPage.values);
            setPageCount(dataPage.pageCount);
        }
    }

    async function onSubmit(event: React.FormEvent) {
        event.preventDefault();

        setPage(1);
        fetchCourses();
    }

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;
        setSearchText(inputElement.value);
    }

    return (
        <MainContainer title="Общедоступные курсы">
            <SearchForm value={searchText} placeholder="Название курсов..." onChange={onChange} onSubmit={onSubmit} />

            <Paginate currentPage={page} pageCount={pageCount} setPage={setPage} />

            <ResultCourseContainer
                loading={loading}
                errorText={error}
                courses={courses}
                onModalClose={resetValues}
            />
        </MainContainer>
    )
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