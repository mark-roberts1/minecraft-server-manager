import { User } from "./User";

class LoggedOnUser extends User {
    constructor() {
        super();
        this.isAuthenticated = false;
        this.setAuthenticated = (user: User, isAuthenticated: boolean) => {};
    }
    
    isAuthenticated: boolean;
    setAuthenticated: (user: User, isAuthenticated: boolean) => void;
}

export default LoggedOnUser;