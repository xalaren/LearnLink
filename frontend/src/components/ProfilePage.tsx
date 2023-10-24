import {useEffect} from "react";
import {ProfileInfoContainer} from "./ProfileInfoContainer.tsx";
import {useAuthorization} from "../hooks/GlobalStateHook.ts";
import {useLogout} from "../hooks/LogoutHook.ts";

export function ProfilePage() {
    const {isAuthorized, user} = useAuthorization();
    const logout = useLogout();

    useEffect(() => {
        if(isAuthorized !== null && !isAuthorized) {
            logout();
        }
    }, [isAuthorized])


    return(
        <main className="main container">
            <div className="inner-container">
                <h2 className="main__title">Профиль: </h2>

                {user &&
                    <ProfileInfoContainer user={user}/>
                }
            </div>
        </main>
    )
}