export interface Role {
    id: string;
    name: string;
    order: number;
}

export class Roles {
    public static readonly Admin: Role = { id: "1", name: "Admin", order: 1 };
    public static readonly User: Role = { id: "2", name: "User", order: 2 };

    public static All: Role[] = [
        Roles.Admin,
        Roles.User,
    ];

    public static FindById(id: string) {
        return this.All.find(x => x.id === id);
    }
}
