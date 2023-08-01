update node_file
SET file_id = q.file_id_2
from(
select
n.id node_id,
f.name,
f2.name,
f.id file_id_1,
f2.id file_id_2
from node n
join tenant_node tn1 on tn1.node_id = n.id and tn1.tenant_id = 6
join tenant_node tn2 on tn2.node_id = n.id and tn2.tenant_id = 1
join node_file nf on nf.node_id = n.id
join "file" f on f.id = nf.file_id 
join tenant_file tf on tf.file_id = f.id and tf.tenant_id = 6
join tenant_file tf2 on tf2.tenant_file_id = tf.tenant_file_id and tf2.tenant_id = 6
join "file" f2 on f2.id = tf2.file_id
where tn1.url_id <> tn2.url_id
and tn1.url_id <> tn1.node_id
and tn2.url_id = tn2.node_id
and f.id < f2.id
and f.id > 1000
order by n.id
) q
where node_file.node_id = q.node_id and node_file.file_id = q.file_id_1