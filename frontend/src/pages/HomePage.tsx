import { useEffect } from "react";
import { useHistoryNavigation } from "../hooks/historyNavigation";
import { Paths } from "../models/paths";
import { MainContainer } from "../components/MainContainer";

function HomePage() {
    const { toNext } = useHistoryNavigation();
    useEffect(() => {
        toNext(Paths.homePath + '/' + 1);
    }, []);

    return (
        <MainContainer>

        </MainContainer>
    );
}

export default HomePage;