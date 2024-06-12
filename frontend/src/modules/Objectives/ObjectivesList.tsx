import { useEffect, useState } from "react";
import { useAppSelector } from "../../hooks/redux";
import { useObjectiveQueries } from "../../hooks/objectiveHooks";
import { Objective } from "../../models/objective";
import ContentList from "../../components/ContentList/ContentList";
import { LocalRole } from "../../models/localRole";
import { useHistoryNavigation } from "../../hooks/historyNavigation";
import { Loader } from "../../components/Loader/Loader";
import { ErrorModal } from "../../components/Modal/ErrorModal";
import ItemLink from "../../components/ItemLink/ItemLink";
import ObjectiveCreateModal from "./ObjectiveCreateModal";

interface IObjectivesListProps {
    lessonId: number;
    localRole?: LocalRole;
}

function ObjectivesList({ lessonId, localRole }: IObjectivesListProps) {
    const [objectives, setObjectives] = useState<Objective[]>();
    const [createModalActive, setCreateModalActive] = useState(false);

    const { accessToken } = useAppSelector(state => state.authReducer);
    const { getObjectivesFromLessonQuery, error, loading, resetValues } = useObjectiveQueries();



    useEffect(() => {
        fetchObjectives();
    }, [accessToken, createModalActive]);


    async function fetchObjectives() {
        if (!accessToken) return;

        const foundObjectives = await getObjectivesFromLessonQuery(lessonId, accessToken);

        if (foundObjectives) {
            setObjectives(foundObjectives);
        }
    }

    return (
        <>
            <ContentList
                title="Задания"
                showButton={localRole?.manageInternalAccess || false}
                onHeadButtonClick={() => setCreateModalActive(true)}>
                <BuildedObjectivesList error={error} onError={resetValues} loading={loading} objectives={objectives} />
            </ContentList>

            <ObjectiveCreateModal active={createModalActive} lessonId={lessonId} onClose={() => setCreateModalActive(false)} />
        </>

    );
}

function BuildedObjectivesList(props: {
    error: string,
    onError: () => void,
    loading: boolean,
    objectives?: Objective[]
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
            {props.objectives && props.objectives.length > 0 ?
                <>{
                    props.objectives.map(objective =>
                        <ItemLink
                            title={objective.title}
                            onClick={() => { }}
                            iconClassName="icon-bolt icon-medium-size"
                            className="content-list__item"
                            key={objective.id}
                            checked={false}
                        />)}
                </>
                :
                <p>Нет доступных заданий</p>
            }
        </>

    );

}

export default ObjectivesList;