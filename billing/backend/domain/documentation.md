1. The .Include() Method (Single Round-Trip Eager Loading)If you are querying for a bill and want to fetch the associated client in the same database trip (similar to EF Core's .Include()), use Marten's .Include() extension: C#Client? client = null;

var bill = await session.Query<Bill>()
.Include(b => b.ClientId, out client) // Tells Marten to fetch the Client where Client.Id == Bill.ClientId
.Where(b => b.Id == targetBillId)
.SingleOrDefaultAsync(cancellationToken);

// Both 'bill' and 'client' are now populated in a single database round-trip 2. LINQ GroupJoin (Multi-Document Queries & Projections)If you want to query a list of bills joined with their clients into a flat result set, use standard LINQ GroupJoin followed by SelectMany: C#var results = await session.Query<Bill>()
.GroupJoin(
session.Query<Client>(),
bill => bill.ClientId,
client => client.Id,
(bill, clients) => new { Bill = bill, Clients = clients }
)
.SelectMany(
x => x.Clients.DefaultIfEmpty(), // Equivalent to a LEFT JOIN
(x, client) => new { x.Bill, Client = client }
)
.Where(x => x.Bill.Status == StatusCodes.Active)
.ToListAsync(cancellationToken); 3. Explicit Separate Loading (Two Queries)If you already loaded the bill and just need to grab its client separately by ID, use standard document loading:C#var bill = await session.LoadAsync<Bill>(billId, cancellationToken);
if (bill != null)
{
var client = await session.LoadAsync<Client>(bill.ClientId, cancellationToken);
}
