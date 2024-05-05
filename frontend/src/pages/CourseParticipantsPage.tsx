import { useParams } from "react-router-dom";
import { MainContainer } from "../components/MainContainer";
import SearchForm from "../components/SearchForm";
import { useState } from "react";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths } from "../models/paths";
import Paginate from "../components/Paginate";
import { User } from "../models/user";
import { Role } from "../models/role";
import ControlNav from "../components/ControlNav";
import UserItem from "../components/UsersList/UserItem";
import { LocalRole } from "../models/localRole";
import { useAppSelector } from "../hooks/redux";

function CourseParticipantsPage() {
    const courseParam = useParams<'courseId'>();
    const pageParam = useParams<'pageNumber'>();

    const [page, setPage] = useState(Number(pageParam.pageNumber));
    const [pageCount, setPageCount] = useState(1);
    const [searchText, setSearchText] = useState('');
    const { user } = useAppSelector(state => state.userReducer);
    const users = [
        new User('nickname', 'lastname', 'name', null, undefined, new Role(0, 'участник', 'member'), 0),
        user
    ];

    const { toNext } = useHistoryNavigation();

    async function fetchParticipants() {

    }

    async function onSubmit(event: React.FormEvent) {
        event.preventDefault();
        navigateToPage(1);
    }

    function onChange(event: React.ChangeEvent) {
        const inputElement = event.target as HTMLInputElement;
        setSearchText(inputElement.value);
    }

    function navigateToPage(nextPage: number) {
        setPage(nextPage);
        toNext(`${Paths.getCourseParticipantsPath(Number(courseParam.courseId))}/${nextPage}`);
    }

    let localRole: LocalRole = {
        id: 0,
        name: 'member',
        sign: 'member',
        viewAccess: true,
        editAccess: true,
        inviteAccess: true,
        kickAccess: true,
        manageInternalAccess: true,
        removeAccess: true,
        isAdmin: true,
    };

    return (
        <MainContainer title="Участники курса">
            <SearchForm
                placeholder="Найти по имени, фамилии или никнейму..."
                onChange={onChange}
                onSubmit={onSubmit}
                value={searchText} />

            <Paginate currentPage={page} pageCount={pageCount} setPage={navigateToPage} />

            <div className="line-end-container">
                <ControlNav>
                    <button className="control-nav__add-button button-gray icon-plus"></button>
                </ControlNav>
            </div>

            <section className="control-list">
                {users.map(user =>
                    <UserItem
                        localRole={localRole}
                        user={user}
                        onEditButtonClick={() => { }}
                        onKickButtonClick={() => { }}
                    />
                )}
            </section>



        </MainContainer>
    );
}

export default CourseParticipantsPage;