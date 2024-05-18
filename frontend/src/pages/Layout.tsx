import { useHistoryNavigation } from "../hooks/historyNavigation";
import { paths } from "../models/paths";
import { useEffect } from "react";
import { MainContainer } from "../components/MainContainer";

function Layout() {
    const { toNext } = useHistoryNavigation();

    useEffect(() => {
        toNext(paths.public());
    }, [])

    return (
        <MainContainer>

        </MainContainer>
    );
}

export default Layout;