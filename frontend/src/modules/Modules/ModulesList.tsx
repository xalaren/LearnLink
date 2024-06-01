import { useEffect, useState } from "react";
import { Course } from "../../models/course";
import { useAppSelector } from "../../hooks/redux";
import { Module } from "../../models/module";
import ItemLink from "../../components/ItemLink/ItemLink";
import ContentList from "../../components/ContentList/ContentList";
import { useGetCourseModules } from "../../hooks/moduleHooks";
import { Loader } from "../../components/Loader/Loader";
import { ErrorModal } from "../../components/Modal/ErrorModal";
import { useHistoryNavigation } from "../../hooks/historyNavigation";
import { paths } from "../../models/paths";
import ModuleCreateModal from "./ModuleCreateModal";
import ControlItemLink from "../../components/ItemLink/ControlItemLink";

interface IModuleListProps {
    course: Course;
}

function ModulesList({ course }: IModuleListProps) {
    const { user } = useAppSelector(state => state.userReducer);
    const [modules, setModules] = useState<Module[]>();
    const { getModulesQuery, error, loading, resetValues } = useGetCourseModules();

    const [createModalActive, setCreateModalActive] = useState(false);

    useEffect(() => {
        fetchModules();
    }, [user, createModalActive]);

    async function fetchModules() {
        if (!user) return;

        const foundModules = await getModulesQuery(course.id, user.id);

        if (foundModules) {
            setModules(foundModules);
        }
    }

    return (
        <>
            <ContentList
                className="content-side__main"
                title="Изучаемые модули"
                showButton={course.localRole?.manageInternalAccess || false}
                onHeadButtonClick={() => setCreateModalActive(true)}>

                <BuildedModulesList error={error} onError={resetValues} loading={loading} modules={modules} courseId={course.id} />
            </ContentList>

            <ModuleCreateModal active={createModalActive} courseId={course.id} onClose={() => setCreateModalActive(false)} />
        </>

    );
}

function BuildedModulesList(props: {
    error: string,
    onError: () => void,
    loading: boolean,
    courseId: number,
    modules?: Module[]
}) {
    const { toNext } = useHistoryNavigation();

    if (props.loading) {
        return (<Loader />);
    }

    if (props.error) {
        return (
            <ErrorModal active={Boolean(props.error)} onClose={props.onError} error={props.error} />
        );
    }

    return (
        <>
            {props.modules && props.modules.length > 0 ?
                <>{
                    props.modules.map(module =>
                        <ItemLink
                            title={module.title}
                            checked={module.completed}
                            onClick={() => toNext(paths.module.view.full(props.courseId, module.id))}
                            iconClassName="icon-module icon-medium-size"
                            className="content-list__item"
                            key={module.id}
                        />)}
                </>
                :
                <p>Нет доступных модулей</p>
            }
        </>

    );

}

export default ModulesList;